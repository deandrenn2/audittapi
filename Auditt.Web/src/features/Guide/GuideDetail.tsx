import { faArrowCircleLeft } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from "react-router-dom";
import { Questions } from "./Questions/Questions";

export const GuideDetail = () => {
    return (
        <div className="w-full">
            <div className="flex space-x-4  p-4">
                <Link to={'/guide'}
                    title='Volver' className="bg-gray-300 hover:bg-gray-300 text-gray-700 hover:text-gray-800 border border-gray-400 hover:border-gray-600 px-4 py-2 rounded font-bold flex items-center transition-all">
                    <FontAwesomeIcon
                        icon={faArrowCircleLeft}
                        className="fa-search top-3 pr-2 font-bold"
                    />Volver
                </Link>
                <h1 className="block text-2xl font-semibold mb-2">Instrumento de Adherencia a GPC</h1>
            </div>
            <div className="w-full bg-white p-4 rounded-lg">
                <Questions/>
            </div>

        </div>
    )
}