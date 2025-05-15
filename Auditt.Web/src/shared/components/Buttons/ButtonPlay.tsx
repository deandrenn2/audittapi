import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlay } from "@fortawesome/free-solid-svg-icons";

interface ButtonPlayProps {
  url: string;
  state?: any;
  className?: string;
  isOpen?: boolean;
}

export const ButtonPlay = ({ url, state, className, isOpen }: ButtonPlayProps) => {
  return (
    <Link
      to={url}
      state={state}
      className={
        className ||
        "w-8 h-8 rounded-full bg-blue-300 border-blue-400 flex items-center justify-center hover:border-blue-500 mr-2 cursor-pointer"
      }
    >
      <FontAwesomeIcon
        icon={faPlay}
        className={`text-blue-500 transition-transform duration-200 ${isOpen ? "rotate-90" : ""}`}
      />
    </Link>
  );
};