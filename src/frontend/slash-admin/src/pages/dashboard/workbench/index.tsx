import Icon from "@/components/icon/icon";
import { Card, CardContent } from "@/ui/card";
import { Text, Title } from "@/ui/typography";
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router";
import { rgbAlpha } from "@/utils/theme";

import BannerCard from "./banner-card";

export default function Workbench() {
	const { companyId } = useParams();
	const navigate = useNavigate();
	const [metrics, setMetrics] = useState({ totalProducts: 0, totalStock: 0, lowStockAlerts: 0 });
	const [companyName, setCompanyName] = useState("");

	useEffect(() => {
		if (companyId) {
			// Fetch metrics
			fetch(`http://localhost:5293/api/dashboard/${companyId}/metrics`)
				.then((res) => {
					if (!res.ok) throw new Error("Error loading metrics");
					return res.json();
				})
				.then((data) => setMetrics(data))
				.catch(console.error);

			// Fetch company name
			fetch(`http://localhost:5293/api/companies`)
				.then((res) => {
					if (!res.ok) throw new Error("Error loading companies");
					return res.json();
				})
				.then((data) => {
					const company = data.find((c: any) => c.id === companyId);
					if (company) setCompanyName(company.name);
				})
				.catch(console.error);
		}
	}, [companyId]);

	let stockColor = "#10b981"; // green > 10
	if (metrics.totalStock === 0) stockColor = "#ef4444"; // red
	else if (metrics.totalStock <= 10) stockColor = "#fbbf24"; // yellow

	const quickStats = [
		{
			icon: "solar:box-outline",
			label: "Total de Productos",
			value: metrics.totalProducts,
			color: "#3b82f6",
		},
		{
			icon: "solar:layers-minimalistic-outline",
			label: "Total de Stock",
			value: metrics.totalStock,
			color: stockColor,
		},
		{
			icon: "solar:danger-triangle-outline",
			label: "Alertas Stock Bajo",
			value: metrics.lowStockAlerts,
			color: "#ef4444",
		},
	];

	return (
		<div className="flex flex-col gap-4 w-full">
			<BannerCard companyName={companyName} />
			
			{/* 顶部3个统计卡片 */}
			<div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
				{quickStats.map((stat) => (
					<Card key={stat.label} className="flex flex-col justify-between h-full">
						<CardContent className="flex flex-col gap-2 p-4">
							<div className="flex items-center gap-2">
								<div className="rounded-lg p-2" style={{ background: rgbAlpha(stat.color, 0.1) }}>
									<Icon icon={stat.icon} size={24} color={stat.color} />
								</div>
								<Text variant="body2" className="font-semibold cursor-default">
									{stat.label}
								</Text>
							</div>
							<div className="flex items-center gap-2 mt-2">
								<Title as="h3" className="text-3xl font-bold cursor-default" style={{ color: stat.color }}>
									{stat.value}
								</Title>
							</div>
						</CardContent>
					</Card>
				))}
			</div>

			<div className="grid grid-cols-1 md:grid-cols-2 gap-4">
				<Card className="hover:shadow-lg transition-all cursor-pointer" onClick={() => navigate(`/inventory/adjustments/${companyId}`)}>
					<CardContent className="flex items-center gap-4 p-6">
						<div className="rounded-full p-3 bg-primary/10">
							<Icon icon="solar:pen-new-square-outline" size={32} className="text-primary" />
						</div>
						<div>
							<Title as="h3" className="text-lg font-bold">Ajustes de Stock</Title>
							<Text variant="body2" className="text-muted-foreground">
								Registrar un ingreso o salida manual (mermas, errores, etc.)
							</Text>
						</div>
					</CardContent>
				</Card>
			</div>
		</div>
	);
}

