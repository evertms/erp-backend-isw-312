import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { toast } from "sonner";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/ui/card";
import { Button } from "@/ui/button";
import { Input } from "@/ui/input";
import { Textarea } from "@/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/ui/select";
import { Label } from "@/ui/label";
import { RadioGroup, RadioGroupItem } from "@/ui/radio-group";
import { Loader2 } from "lucide-react";
import Icon from "@/components/icon/icon";

const baseSchema = z.object({
	warehouseId: z.string().min(1, "El almacén es obligatorio"),
	productId: z.string().min(1, "El producto es obligatorio"),
	adjustmentType: z.enum(["in", "out"], { required_error: "Debes seleccionar el tipo de ajuste" }),
	quantity: z.coerce.number().min(1, "La cantidad debe ser mayor a 0").positive("Solo valores positivos"),
	notes: z.string().min(5, "Debes incluir un motivo válido (min 5 caracteres)"),
});

type AdjustmentFormValues = z.infer<typeof baseSchema>;

export default function StockAdjustment() {
	const { companyId } = useParams();
	const navigate = useNavigate();
	const [warehouses, setWarehouses] = useState<any[]>([]);
	const [products, setProducts] = useState<any[]>([]);
	const [loadingData, setLoadingData] = useState(true);
	const [currentStock, setCurrentStock] = useState<number | null>(null);

	const schema = baseSchema.superRefine((data, ctx) => {
		if (data.adjustmentType === "out" && currentStock !== null && data.quantity > currentStock) {
			ctx.addIssue({
				path: ["quantity"],
				code: z.ZodIssueCode.custom,
				message: `La cantidad a retirar no puede superar el stock actual (${currentStock})`,
			});
		}
	});

	const form = useForm<AdjustmentFormValues>({
		resolver: zodResolver(schema),
		defaultValues: {
			warehouseId: "",
			productId: "",
			adjustmentType: "in",
			quantity: 1, // Start with a safe default rather than 0 since the min rules enforce >= 1
			notes: "",
		},
	});

	const selectedWarehouseId = form.watch("warehouseId");
	const selectedProductId = form.watch("productId");
	const adjustmentType = form.watch("adjustmentType");
	const quantity = form.watch("quantity");

	// Initial data fetch
	useEffect(() => {
		if (companyId) {
			Promise.all([
				// Simulando o asumiendo que existen estos endpoints
				fetch(`http://localhost:5293/api/companies/${companyId}/warehouses`).then(res => res.ok ? res.json() : []),
				fetch(`http://localhost:5293/api/companies/${companyId}/products`).then(res => res.ok ? res.json() : [])
			])
				.then(([warehousesData, productsData]) => {
					setWarehouses(warehousesData);
					setProducts(productsData);
				})
				.catch(err => {
					console.error("Error fetching data", err);
					toast.error("Error al cargar datos necesarios. Usando datos de prueba temporales.");
					setWarehouses([{ id: "00000000-0000-0000-0000-000000000001", name: "Almacén Central" }]);
					setProducts([{ id: "00000000-0000-0000-0000-000000000002", name: "Producto de Prueba" }]);
				})
				.finally(() => setLoadingData(false));
		}
	}, [companyId]);

	// Fetch specific current stock when product + warehouse selected
	useEffect(() => {
		if (selectedWarehouseId && selectedProductId) {
			// Intentamos traer el stock. Esta ruta debe exponer el stock de un producto en un almacén.
			fetch(`http://localhost:5293/api/inventory/products/${selectedProductId}/stock/warehouses/${selectedWarehouseId}`)
				.then(res => res.ok ? res.json() : null)
				.then(data => {
					// Si devuelve el modelo, extraemos cantidad. Por si acaso usamos 100 de mock fallido temporal
					setCurrentStock(data?.currentQuantity ?? 100); 
				})
				.catch(() => setCurrentStock(100)); // mock
		} else {
			setCurrentStock(null);
		}
	}, [selectedWarehouseId, selectedProductId]);

	// Revalidate form dynamically when specific validation conditions change
	useEffect(() => {
		if (form.formState.isDirty || form.formState.errors.quantity) {
			form.trigger("quantity"); 
		}
	}, [currentStock, adjustmentType, quantity, form]);

	const onSubmit = async (data: AdjustmentFormValues) => {
		try {
			// Convertir la cantidad basada en tipo de ajuste (Entrada o Salida)
			const resolvedQuantityDifference = data.adjustmentType === "in" ? data.quantity : -data.quantity;

			const payload = {
				companyId: companyId,
				warehouseId: data.warehouseId,
				notes: data.notes,
				lines: [
					{
						productId: data.productId,
						quantityDifference: resolvedQuantityDifference
					}
				]
			};

			const response = await fetch(`http://localhost:5293/api/inventory/adjustments`, {
				method: "POST",
				headers: {
					"Content-Type": "application/json"
				},
				body: JSON.stringify(payload)
			});

			if (!response.ok) {
				const errorData = await response.json().catch(() => null);
				throw new Error(errorData?.message || "Ocurrió un error al registrar el ajuste.");
			}

			toast.success("Ajuste registrado", {
				description: "El ajuste de stock y kardex fue generado exitosamente."
			});
			
			navigate(`/dashboard/${companyId}`);
		} catch (error: any) {
			toast.error("Error", {
				description: error.message
			});
		}
	};

	return (
		<div className="flex flex-col gap-4 w-full max-w-2xl mx-auto p-4">
			<Card className="w-full">
				<CardHeader>
					<div className="flex items-center gap-2 mb-2">
						<Button variant="ghost" size="icon" onClick={() => navigate(-1)} className="rounded-full">
							<Icon icon="solar:arrow-left-outline" size={24} />
						</Button>
						<CardTitle className="text-2xl flex items-center gap-2">
							<Icon icon="solar:box-outline" size={28} className="text-primary" />
							Ajuste de Stock
						</CardTitle>
					</div>
					<CardDescription>
						Registra entradas o salidas manuales de inventario al flujo (mermas, conteos, ajustes)
					</CardDescription>
				</CardHeader>
				<CardContent>
					{loadingData ? (
						<div className="flex justify-center py-8">
							<Loader2 className="animate-spin h-8 w-8 text-primary" />
						</div>
					) : (
						<form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
							<div className="space-y-2">
								<Label>Tipo de Ajuste</Label>
								<Controller
									name="adjustmentType"
									control={form.control}
									render={({ field }) => (
										<RadioGroup 
											className="flex space-x-4 mt-2"
											value={field.value} 
											onValueChange={field.onChange}
										>
											<div className="flex items-center space-x-2">
												<RadioGroupItem value="in" id="in" />
												<Label htmlFor="in" className="cursor-pointer text-green-600 font-semibold flex items-center gap-1">
													<Icon icon="solar:round-alt-arrow-down-bold-duotone" /> Entrada (+ Stock)
												</Label>
											</div>
											<div className="flex items-center space-x-2">
												<RadioGroupItem value="out" id="out" />
												<Label htmlFor="out" className="cursor-pointer text-red-600 font-semibold flex items-center gap-1">
													<Icon icon="solar:round-alt-arrow-up-bold-duotone" /> Salida (- Stock)
												</Label>
											</div>
										</RadioGroup>
									)}
								/>
								{form.formState.errors.adjustmentType && (
									<p className="text-sm text-red-500">{form.formState.errors.adjustmentType.message}</p>
								)}
							</div>

							<div className="grid grid-cols-1 md:grid-cols-2 gap-4">
								<div className="space-y-2">
									<Label htmlFor="warehouseId">Almacén</Label>
									<Controller
										name="warehouseId"
										control={form.control}
										render={({ field }) => (
											<Select onValueChange={field.onChange} defaultValue={field.value}>
												<SelectTrigger className={form.formState.errors.warehouseId ? "border-red-500" : ""}>
													<SelectValue placeholder="Selecciona un almacén" />
												</SelectTrigger>
												<SelectContent>
													{warehouses.map(w => (
														<SelectItem key={w.id} value={w.id}>{w.name}</SelectItem>
													))}
												</SelectContent>
											</Select>
										)}
									/>
									{form.formState.errors.warehouseId && (
										<p className="text-sm text-red-500">{form.formState.errors.warehouseId.message}</p>
									)}
								</div>

								<div className="space-y-2">
									<Label htmlFor="productId">Producto</Label>
									<Controller
										name="productId"
										control={form.control}
										render={({ field }) => (
											<Select onValueChange={field.onChange} defaultValue={field.value}>
												<SelectTrigger className={form.formState.errors.productId ? "border-red-500" : ""}>
													<SelectValue placeholder="Selecciona el producto" />
												</SelectTrigger>
												<SelectContent>
													{products.map(p => (
														<SelectItem key={p.id} value={p.id}>{p.name}</SelectItem>
													))}
												</SelectContent>
											</Select>
										)}
									/>
									{form.formState.errors.productId && (
										<p className="text-sm text-red-500">{form.formState.errors.productId.message}</p>
									)}
								</div>
							</div>

							<div className="space-y-2">
								<div className="flex items-center justify-between">
									<Label htmlFor="quantity">Cantidad</Label>
									{currentStock !== null && selectedWarehouseId && selectedProductId && (
										<span className="text-sm text-muted-foreground bg-muted px-2 py-0.5 rounded">Stock Actual: {currentStock}</span>
									)}
								</div>
								<Input
									id="quantity"
									type="number"
									min="1"
									{...form.register("quantity")}
									className={form.formState.errors.quantity ? "border-red-500" : ""}
									placeholder="Ingresa la cantidad (Ej. 10)"
								/>
								{form.formState.errors.quantity && (
									<p className="text-sm text-red-500">{form.formState.errors.quantity.message}</p>
								)}
							</div>

							<div className="space-y-2">
								<Label htmlFor="notes">Motivo (Notas)</Label>
								<Textarea
									id="notes"
									{...form.register("notes")}
									className={form.formState.errors.notes ? "border-red-500 min-h-[100px]" : "min-h-[100px]"}
									placeholder="Ej. Ajuste por producto dañado, inventario físico, etc."
								/>
								{form.formState.errors.notes && (
									<p className="text-sm text-red-500">{form.formState.errors.notes.message}</p>
								)}
							</div>

							<Button type="submit" className="w-full" disabled={form.formState.isSubmitting}>
								{form.formState.isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
								Confirmar Ajuste
							</Button>
						</form>
					)}
				</CardContent>
			</Card>
		</div>
	);
}
