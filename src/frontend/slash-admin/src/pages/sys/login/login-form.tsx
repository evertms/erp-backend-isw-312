import { Button } from "@/ui/button";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/ui/select";
import { Skeleton } from "@/ui/skeleton";
import { useUserActions } from "@/store/userStore";
import { cn } from "@/utils";
import { Loader2 } from "lucide-react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { toast } from "sonner";
import fallbackAvatar from "@/assets/images/characters/character_3.png";

type Company = {
	id: string;
	name: string;
	imageUrl: string | null;
};

export function LoginForm({ className, ...props }: React.ComponentPropsWithoutRef<"form">) {
	const [loading, setLoading] = useState(false);
	const [fetching, setFetching] = useState(true);
	const [companies, setCompanies] = useState<Company[]>([]);
	const [selectedCompanyId, setSelectedCompanyId] = useState<string>("");
	
	const navigate = useNavigate();
	const { setUserToken, setUserInfo } = useUserActions();

	useEffect(() => {
		// Llamada a la API de empresas (US-01)
		fetch("http://localhost:5293/api/companies")
			.then((res) => {
				if (!res.ok) throw new Error("Fallo al cargar las empresas");
				return res.json();
			})
			.then((data: Company[]) => {
				setCompanies(data);
			})
			.catch((err) => {
				toast.error("Error conectando con el servidor", {
					description: err.message,
				});
			})
			.finally(() => {
				setFetching(false);
			});
	}, []);

	const handleFinish = async (e: React.FormEvent) => {
		e.preventDefault();
		
		if (!selectedCompanyId) {
			toast.error("Por favor selecciona una empresa primero");
			return;
		}

		setLoading(true);
		
		try {
			const company = companies.find(c => c.id === selectedCompanyId);
			if (!company) throw new Error("Empresa no encontrada");

			// Simulamos el Login usando el CompanyID como token de acceso y un usuario genérico
			setUserToken({ accessToken: selectedCompanyId, refreshToken: selectedCompanyId });
			
			setUserInfo({ 
				id: selectedCompanyId, 
				username: "Operador de Inventario",
				email: "operador@empresa.com",
				avatar: company.imageUrl || fallbackAvatar,
				roles: ["admin"] as any,
				permissions: ["sys.menu.dashboard"] as any
			});
			
			// Redirige al Dashboard usando el ID
			navigate(`/dashboard/${selectedCompanyId}`, { replace: true });
			
			toast.success("Empresa seleccionada con éxito", {
				description: `Ingresando al inventario de ${company.name}`
			});
		} catch (err: any) {
			toast.error(err.message, {
				position: "top-center",
			});
		} finally {
			setLoading(false);
		}
	};

	return (
		<div className={cn("flex flex-col gap-6", className)}>
			<form onSubmit={handleFinish} className="space-y-6" {...props}>
				<div className="flex flex-col items-center gap-2 text-center">
					<h1 className="text-2xl font-bold">Bienvenido</h1>
					<p className="text-balance text-sm text-muted-foreground">
						Selecciona una empresa para continuar
					</p>
				</div>

				<div className="space-y-4">
					{fetching ? (
						<Skeleton className="h-10 w-full rounded-md" />
					) : (
						<Select value={selectedCompanyId} onValueChange={setSelectedCompanyId}>
							<SelectTrigger className="w-full">
								<SelectValue placeholder="Seleccione una compañía..." />
							</SelectTrigger>
							<SelectContent>
								{companies.map((company) => (
									<SelectItem key={company.id} value={company.id}>
										{company.name}
									</SelectItem>
								))}
							</SelectContent>
						</Select>
					)}
				</div>

				{/* Botón Ingresar */}
				<Button type="submit" className="w-full" disabled={fetching || loading || !selectedCompanyId}>
					{loading && <Loader2 className="animate-spin mr-2" />}
					Ingresar al Sistema
				</Button>
			</form>
		</div>
	);
}

export default LoginForm;
