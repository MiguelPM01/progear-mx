import { NavLink } from "react-router";

function obtenerClasesEnlace(isActive: boolean) {
    return isActive
    ? "block w-full text-left bg-primary text-white px-3 py-3 rounded-lg transition-colors"
    : "block w-full text-left bg-transparent text-secondary px-3 py-3 rounded-lg hover:bg-white/10 transition-colors";
}

function Sidebar() {
    return(
        <nav className="w-70 p-4 rounded-xl bg-surface font-sans text-secondary">
            <div className="mb-6 flex items-center gap-3">
                <div className="flex h-12 w-12 items-center justify-center rounded-lg border border-secondary">
                    Logo
                </div>

                <div>
                    <h2 className="font-display text-xl font-bold">
                        ProGear MX
                    </h2>
                    <p className="text-xs text-secondary">
                        ENGINEERING · INVENTORY
                    </p>
                </div>
            </div>

            <ul className="flex flex-col gap-2">
                <li>
                    <NavLink
                    to="/"
                    className={({ isActive }) => obtenerClasesEnlace(isActive)}
                    >
                            Inicio
                    </NavLink>
                </li>

                <li>
                    <NavLink
                    to="/productos"
                    className={({ isActive }) => obtenerClasesEnlace(isActive)}
                        >
                            Productos
                    </NavLink>
                </li>

                <li>
                    <NavLink
                    to="/inventario"
                    className={({ isActive }) => obtenerClasesEnlace(isActive)}
                        >
                            Inventario
                    </NavLink>
                </li>
                
                <li>
                    <NavLink
                    to="/precios"
                    className={({ isActive }) => obtenerClasesEnlace(isActive)}
                        >
                            Precios
                    </NavLink>
                </li>
            </ul>
        </nav>
    )
}

export default Sidebar

