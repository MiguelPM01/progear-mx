import type { LucideIcon } from "lucide-react";

type KpiCardProps= {
    id:string;
    titulo: string;
    valor: number;
    icono: LucideIcon
};

function KpiCard(props: KpiCardProps) {
    const Icono = props.icono;

    return (
        <article className="flex flex-col gap-3 rounded-2xl bg-surface p-5">
            <div className="flex items-center gap-2">
                <Icono size={24} color="#2589FF"/>
                <h2 className="font-sans">{props.titulo}</h2>
            </div>

            <p className="font-display font-semibold text-primary text-5xl">
                {props.valor}
            </p>

            <p className="text-sm text-secondary font-display">
                {props.id}
            </p>

        </article>
    );
}

export default KpiCard

