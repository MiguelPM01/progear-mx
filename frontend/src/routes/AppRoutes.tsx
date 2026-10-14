import { Routes, Route } from "react-router";
import DashboardPage from "../pages/DashboardPage";
import ProductosPage from "../pages/ProductosPage";
import InventarioPage from "../pages/InventarioPage";
import PreciosPage from "../pages/PreciosPage";
import NotFoundPage from "../pages/NotFoundPage";

function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<DashboardPage/>} />
           <Route path="/productos" element={<ProductosPage />} />
           <Route path="/inventario" element={<InventarioPage/>} />
           <Route path="/precios" element={<PreciosPage/>} />
           <Route path="*" element={<NotFoundPage />} />

        </Routes>
    );
}

export default AppRoutes



