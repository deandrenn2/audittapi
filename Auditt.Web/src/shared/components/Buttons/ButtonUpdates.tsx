import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';

interface DetailButtonProps {

  state?: any;
  className?: string;
  xClick?: () => void;
}

const ButtonUpdates = ({  className, xClick }: DetailButtonProps) => {
  const handleClick = () => {
    if (xClick)
      xClick();
  };
  return (
    <button
    type="button"
    onClick={handleClick}
    className={className || "w-6 h-6 rounded-full bg-green-300 border border-green-400 flex items-center justify-center hover:border-green-500 mr-2 cursor-pointer"}
  >
    <FontAwesomeIcon icon={faPen} className="text-green-500" />
  </button>
    
  );
};
export default ButtonUpdates;