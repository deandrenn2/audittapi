import { faPlay } from "@fortawesome/free-solid-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"

export const ButtonPlay = () =>{
    return (
        <button className="w-9 h-9 rounded-full bg-blue-300  border-blue-400 flex items-center justify-center hover:border-blue-500 mr-2">
              <FontAwesomeIcon icon={faPlay} className="text-blue-500" />
        </button>
    )
}