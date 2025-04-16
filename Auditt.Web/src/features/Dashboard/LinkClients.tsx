import { Link, useLocation } from "react-router-dom";

export const LinkClients = () => {
    const location = useLocation(); 

    return (
        <div className="flex gap-4">
            <Link
                to="/Clients"
                className={`text-gray-800 border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Clients"
                        ? "text-pink-500 border-pink-500": ""}`}>
                Clientes
            </Link>

            <Link
                to="/patients"
                className={`text-gray-800 border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/patients"
                        ? "text-pink-500 border-pink-500": ""}`}>
                Pacientes
            </Link>

            <Link
                to="/Professionals"
                className={`text-gray-800 border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Professionals"
                        ? "text-pink-500 border-pink-500" : ""}`}>
                Profesionales
            </Link>
        </div>
    );
};


