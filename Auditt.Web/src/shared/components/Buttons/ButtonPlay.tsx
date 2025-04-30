import { faPlay } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from "react-router-dom";

interface DetailButtonProps {
    url: string;
    state?: any;
    className?: string;
    xClick?: () => void;
}
export const ButtonPlay = ({ url, state, className, xClick }: DetailButtonProps) => {
    const handleClick = () => {
        if (xClick)
            xClick();
    };

    return (
        <Link
            to={url}
            state={state}
            className={className || "w-8 h-8 rounded-full bg-blue-300  border-blue-400 flex items-center justify-center hover:border-blue-500 mr-2"}
            onClick={handleClick}
        >
            <FontAwesomeIcon icon={faPlay} className='text-blue-500' />
        </Link>
    )
}