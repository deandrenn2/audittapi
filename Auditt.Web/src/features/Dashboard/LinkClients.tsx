import { Link, useLocation } from "react-router-dom";

export const LinkClients = () => {
    const location = useLocation(); 

    return (
        <div className="flex gap-4">
            <Link
                to="/Clients"
                className={`border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Clients"
                        ? "text-[#FF677D] border-[#FF677D] text-2xl"
                        : "text-gray-800"
                }`}>
                Clientes
            </Link>

            <Link
                to="/patients"
                className={`border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/patients"
                        ? "text-[#FF677D] border-[#FF677D] text-2xl"
                        : "text-gray-800"
                }`}>
                Pacientes
            </Link>

            <Link
                to="/Functionary"
                className={`border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Functionary"
                        ? "text-[#FF677D] border-[#FF677D] text-2xl"
                        : "text-gray-800"
                }`}>
                Profesionales
            </Link>
        </div>
    );
};
