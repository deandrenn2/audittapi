import { Link, useLocation } from "react-router-dom";

export const LinkSettings = () => {
    const location = useLocation();

    return (
        <div className="flex gap-4">
            <Link
                to="/Users"
                className={`border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Users" ? "text-[#FF677D] border-[#FF677D]" : "text-gray-800"
                }`}>
                Usuario
            </Link>

            <Link
                to="/Roles"className={`border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Roles" ? "text-[#FF677D] border-[#FF677D]": "text-gray-800"
                }`}>
                Roles
            </Link>

            <Link
                to="/Scales" className={`border-b-2 pb-1 px-2 transition-colors duration-300 ${
                    location.pathname === "/Scales" ? "text-[#FF677D] border-[#FF677D]": "text-gray-800"
                }`}>
                Escalas
            </Link>
        </div>
    );
}
