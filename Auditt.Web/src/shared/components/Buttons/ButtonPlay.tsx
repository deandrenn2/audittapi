import { faPlay } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

interface ButtonPlayProps {
    xClick?: () => void;
    className?: string;
    isOpen?: boolean; // opcional, para mostrar diferente ícono
}

export const ButtonPlay = ({ xClick, className, isOpen }: ButtonPlayProps) => {
    return (
        <button
            onClick={xClick}
            className={className || "w-8 h-8 rounded-full bg-blue-300 border-blue-400 flex items-center justify-center hover:border-blue-500 mr-2"}
        >
            <FontAwesomeIcon icon={faPlay} className={`text-blue-500 transition-transform duration-200 ${isOpen ? 'rotate-90' : ''}`} />
        </button>
    );
};
