import { Link } from "react-router-dom";
export const LinkClients = () => {
    return (
        <div>
            <Link to="/Clients" className="text-gray-800 border-b-2 border-gray-800 pb-1 mr-4">
                Clientes
            </Link>

            <Link to="/patients" className="text-gray-800 border-b-2 border-gray-800 pb-1 mr-4">
                Pacientes
            </Link>
            <Link to="/Professionals" className="text-gray-800 border-b-2 border-gray-800 pb-1 mr-4">
                Profesionales
            </Link>
        </div>
    );
};