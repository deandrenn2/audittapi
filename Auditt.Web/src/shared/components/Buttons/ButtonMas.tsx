import { faPlus } from "@fortawesome/free-solid-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"

export const ButtonPlus = () =>{
    return (
        <div>
            <button className="w-9 h-9 rounded-full bg-green-300  border-green-400 flex items-center justify-center hover:border-green-500 mr-2">
            <FontAwesomeIcon icon={faPlus} className="text-green-500" />
            </button>
        </div>
    )
} 