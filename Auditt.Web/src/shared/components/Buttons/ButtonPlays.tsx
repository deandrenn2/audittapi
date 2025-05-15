import { faPlay } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate } from "react-router-dom"; // 👈 importar navegación

interface ButtonPlayProps {
    xClick?: () => void;
    className?: string;
    isOpen?: boolean;
    url?: string; 
}

export const ButtonPlays = ({ xClick, className, isOpen, url }: ButtonPlayProps) => {
    const navigate = useNavigate();

    const handleClick = () => {
        if (url) {
            navigate(`/${url}`); 
        }

        if (xClick) xClick(); 
    };

    return (
        <div>
            <button
                onClick={handleClick}
                className={
                    className ||
                    "w-8 h-8 rounded-full bg-blue-300 border-blue-400 flex items-center justify-center hover:border-blue-500 mr-2 cursor-pointer"
                }
            >
                <FontAwesomeIcon
                    icon={faPlay}
                    className={`text-blue-500 transition-transform duration-200 ${isOpen ? 'rotate-90' : ''}`}
                />
            </button>
        </div>
    );
};
