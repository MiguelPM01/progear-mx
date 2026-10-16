import Header from "./components/layout/Header";
import AppRoutes from "./routes/AppRoutes";
import Sidebar from "./components/layout/Sidebar";
import { useState } from "react";

function App() {
  const [sidebarVisible, setSidebarVisible] = useState(true);

  function toggleSidebar() {
    setSidebarVisible((visible) => !visible);
  }

  return (
    <main className="min-h-screen p-8 flex flex-col gap-6">
      <Header onToggleSidebar={toggleSidebar}/>

      <div className="flex gap-6">
        {sidebarVisible && <Sidebar />}

        <div className="flex-1">
          <AppRoutes/>
        </div>
      </div>
    </main>
  );
}

export default App