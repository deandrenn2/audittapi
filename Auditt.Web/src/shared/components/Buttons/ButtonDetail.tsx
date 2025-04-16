import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';

interface DetailButtonProps {
  url: string;
  state?: any;
  className?: string;
  xClick?: () => void;
}

const ButtonDetail = ({ url, state, className, xClick }: DetailButtonProps) => {
  const handleClick = () => {
    if (xClick)
      xClick();
  };
  return (
    <Link
      to={url}
      state={state}
      className={className || "w-10 h-10 rounded-full bg-green-300  border-green-400 flex items-center justify-center hover:border-green-500 mr-2"}
      onClick={handleClick}
    >
      <FontAwesomeIcon icon={faPen} className='text-green-500' />
    </Link>
  );
};

export default ButtonDetail;