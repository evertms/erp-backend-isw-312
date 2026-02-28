import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/ui/select";
import { Button } from "@/ui/button";
import { Label } from "@/ui/label";
import { Loader2 } from "lucide-react";
import Icon from "@/components/icon/icon";
import { toast } from "sonner";
import { format } from "date-fns";

type Product = {
	id: string;
	name: string;
};

type KardexMovement = {
	movementType: string;
	movementDate: string | null;
	quantity: number;
	balance: number;
	reason: string | null;
};

export default function KardexView() {
	const { companyId } = useParams();
	const navigate = useNavigate();
	
	const [products, setProducts] = useState<Product[]>([]);
	const [selectedProductId, setSelectedProductId] = useState<string>("");
	const [movements, setMovements] = useState<KardexMovement[]>([]);
	
	const [loadingProducts, setLoadingProducts] = useState(true);
	const [loadingKardex, setLoadingKardex] = useState(false);

	// Fetch products for the company
	useEffect(() => {
		if (companyId) {
			fetch(`http://localhost:5293/api/companies/${companyId}/products`)
				.then((res) => (res.ok ? res.json() : []))
				.then((data) => {
					setProducts(data);
				})
				.catch((err) => {
					console.error("Error fetching products", err);
					toast.error("Error al cargar la lista de productos.");
					// Fallback mock
					setProducts([{ id: "00000000-0000-0000-0000-000000000002", name: "Producto de Prueba" }]);
				})
				.finally(() => setLoadingProducts(false));
		}
	}, [companyId]);

	// Fetch Kardex when a product is selected
	useEffect(() => {
		if (selectedProductId) {
			setLoadingKardex(true);
			fetch(`http://localhost:5293/api/inventory/products/${selectedProductId}/kardex`)
				.then((res) => {
					if (!res.ok) throw new Error("Fallo al cargar kardex");
					return res.json();
				})
				.then((data) => {
					setMovements(data);
				})
				.catch((err) => {
					console.error("Error fetching kardex", err);
					toast.error("Error al cargar los movimientos del producto.");
					setMovements([]);
				})
				.finally(() => setLoadingKardex(false));
		} else {
			setMovements([]);
		}
	}, [selectedProductId]);

	// Format date nicely
	const formatDate = (dateString?: string | null) => {
		if (!dateString) return "-";
		try {
			return format(new Date(dateString), "dd MMM yyyy HH:mm");
		} catch {
			return dateString;
		}
	};

	return (
		<div className="flex flex-col gap-4 w-full max-w-5xl mx-auto p-4">
			<Card className="w-full">
				<CardHeader className="flex flex-row items-center justify-between pb-2">
					<div className="flex items-center gap-2">
						<Button variant="ghost" size="icon" onClick={() => navigate(-1)} className="rounded-full">
							<Icon icon="solar:arrow-left-outline" size={24} />
						</Button>
						<div>
							<CardTitle className="text-2xl flex items-center gap-2">
								<Icon icon="solar:history-bold-duotone" size={28} className="text-primary" />
								Kardex de Producto
							</CardTitle>
							<CardDescription className="mt-1">
								Historial de movimientos, ajustes y estado del inventario
							</CardDescription>
						</div>
					</div>
				</CardHeader>
				
				<CardContent>
					<div className="space-y-6">
						{/* Product Selector */}
						<div className="max-w-md space-y-2">
							<Label htmlFor="productId">Selecciona un Producto</Label>
							{loadingProducts ? (
								<div className="flex items-center text-sm text-muted-foreground">
									<Loader2 className="mr-2 h-4 w-4 animate-spin" /> Cargando productos...
								</div>
							) : (
								<Select value={selectedProductId} onValueChange={setSelectedProductId}>
									<SelectTrigger id="productId">
										<SelectValue placeholder="Selecciona el producto para ver su historial" />
									</SelectTrigger>
									<SelectContent>
										{products.map((p) => (
											<SelectItem key={p.id} value={p.id}>{p.name}</SelectItem>
										))}
									</SelectContent>
								</Select>
							)}
						</div>

						{/* Kardex List / Table */}
						{selectedProductId && (
							<div className="mt-6 border rounded-md overflow-hidden">
								{loadingKardex ? (
									<div className="flex justify-center py-12">
										<Loader2 className="animate-spin h-8 w-8 text-primary" />
									</div>
								) : movements.length === 0 ? (
									<div className="flex flex-col items-center justify-center py-12 text-muted-foreground">
										<Icon icon="solar:box-minimalistic-outline" size={48} className="mb-2 opacity-50" />
										<p>No hay movimientos registrados para este producto.</p>
									</div>
								) : (
									<div className="overflow-x-auto w-full">
										<table className="w-full text-sm text-left">
											<thead className="bg-muted text-muted-foreground">
												<tr>
													<th className="px-4 py-3 font-semibold">Fecha</th>
													<th className="px-4 py-3 font-semibold">Tipo de Movimiento</th>
													<th className="px-4 py-3 font-semibold">Motivo</th>
													<th className="px-4 py-3 font-semibold text-right">Cantidad</th>
													<th className="px-4 py-3 font-semibold text-right">Balance</th>
												</tr>
											</thead>
											<tbody className="divide-y">
												{movements.map((mov, index) => (
													<tr key={index} className="hover:bg-muted/50 transition-colors">
														<td className="px-4 py-3 whitespace-nowrap">{formatDate(mov.movementDate)}</td>
														<td className="px-4 py-3 font-medium">
															<span className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-semibold ${
																mov.quantity > 0 ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400" :
																mov.quantity < 0 ? "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400" :
																"bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-400"
															}`}>
																{mov.movementType || "Ajuste"}
															</span>
														</td>
														<td className="px-4 py-3 max-w-xs truncate" title={mov.reason || ""}>
															{mov.reason || "-"}
														</td>
														<td className={`px-4 py-3 text-right font-bold ${
															mov.quantity > 0 ? "text-green-600 dark:text-green-400" :
															mov.quantity < 0 ? "text-red-600 dark:text-red-400" : ""
														}`}>
															{mov.quantity > 0 ? "+" : ""}{mov.quantity}
														</td>
														<td className="px-4 py-3 text-right font-bold border-l bg-accent/20">
															{mov.balance}
														</td>
													</tr>
												))}
											</tbody>
										</table>
									</div>
								)}
							</div>
						)}
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
