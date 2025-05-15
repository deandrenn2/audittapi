import { faPen } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export const ButtonUpdate = () => {
  return (
    <div>
      <button className="w-8 h-8 rounded-full bg-green-300  cursor-pointer border-green-400 flex items-center justify-center hover:border-green-500 mr-2">
        <FontAwesomeIcon icon={faPen} className="text-green-500" />
      </button>
    </div>
  )
}
