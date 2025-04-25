import { Link } from "react-router-dom"
export const LinkSettings = () => {
    return (
        <div className="flex gap-4">
            <Link
                to="/Users"
                className={`text-gray-800 border-b-2 pb-1 px-2 transition-colors duration-300 ${location.pathname === "/Users"
                    ? "text-pink-500 border-pink-500" : ""}`}>
                Usuario
            </Link>

            <Link
                to="/Roles"
                className={`text-gray-800 border-b-2 pb-1 px-2 transition-colors duration-300 ${location.pathname === "/Roles"
                    ? "text-pink-500 border-pink-500" : ""}`}>
                Roles
            </Link>

            <Link
                to="/Scales"
                className={`text-gray-800 border-b-2 pb-1 px-2 transition-colors duration-300 ${location.pathname === "/Scales"
                    ? "text-pink-500 border-pink-500" : ""}`}>
                Escalas
            </Link>
        </div>
    )
}