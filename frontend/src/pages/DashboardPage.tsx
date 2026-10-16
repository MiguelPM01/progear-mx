import { Package, Warehouse, Gauge, Lock } from "lucide-react";
import KpiCard from "../components/dashboard/KpiCard"

function DashboardPage () {
    return(
        <main>
            <div>
                <p className="text-sm text-secondary font-semibold font-sans">
                    PANEL PRINCIPAL / INVENTARIO
                </p>
                <h1 className="text-5xl text-primary font-bold font-display">
                    Resumen General
                </h1>

                <p className="text-secondary text-sm font-semibold font-sans">
                    Consulta el estado de tus productos y las existencias de tu tienda.
                </p>
            </div>

            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
            <KpiCard
                id={"KPI / 01"}
                titulo="Productos registrados"
                valor={120}
                icono={Package}
                />

            <KpiCard
                id={"KPI / 02"}
                titulo= "Existencias físicas"
                valor={120} 
                icono={Warehouse}
                />
            <KpiCard 
                id={"KPI / 03"}
                titulo="Reservado"
                valor={20}
                icono={Lock}
                />

            <KpiCard
                id={"KPI / 04"}
                titulo="Disponible"
                valor={100} 
                icono={Gauge}
                />

            </div>
        </main>
    )
}

export default DashboardPage

