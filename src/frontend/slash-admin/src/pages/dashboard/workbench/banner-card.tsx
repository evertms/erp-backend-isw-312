import bgImg from "@/assets/images/background/banner-1.png";
import type { CSSProperties } from "react";

type Props = {
	companyName: string;
};

export default function BannerCard({ companyName }: Props) {
	const bgStyle: CSSProperties = {
		position: "absolute",
		top: 0,
		left: 0,
		right: 0,
		bottom: 0,
		backgroundImage: `url("${bgImg}")`,
		backgroundSize: "100%",
		backgroundPosition: "50%",
		backgroundRepeat: "no-repeat",
		opacity: 0.5,
	};
	return (
		<div className="relative bg-primary/90 rounded-2xl overflow-hidden">
			<div className="p-6 z-2 relative">
				<div className="flex flex-col gap-4">
					<h1 className="text-white text-3xl font-bold">
						{companyName || "Inventario de Empresa"}
					</h1>
					<p className="text-white">
						Bienvenido al dashboard de control de inventario.
					</p>
				</div>
			</div>
			<div style={bgStyle} className="z-1" />
		</div>
	);
}
