import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencil } from '@fortawesome/free-solid-svg-icons';


interface DetailButtonProps {
  url: string;
  state?: any;
  className?: string;
}

const ButtonDetail = ({ url, state, className }: DetailButtonProps) => {
  return (
    <Link
      to={url}
      state={state}
      className={className || "w-10 h-10 rounded-full bg-green-300  border-green-400 flex items-center justify-center hover:border-green-500 mr-2"}
    >
      <FontAwesomeIcon icon={faPencil}className='text-green-500' />
    </Link>
  );
};

export default ButtonDetail;