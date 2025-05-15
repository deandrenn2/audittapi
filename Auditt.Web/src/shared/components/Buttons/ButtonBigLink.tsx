import { faLink } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from "react-router-dom";

export const ButtonBigLink = ({ text, to, className }: { text: string, to?: string, className?: string }) => {
    return (
        <Link className={`bg-audittpurple text-white text-2xl rounded-4xl font-semibold mb-3 mr-2 p-8 hover:bg-purple-800 transition-all ${className}`} to={to ?? "#"}>
            {text} <FontAwesomeIcon icon={faLink} className="ml-2" />
        </Link>
    );
};