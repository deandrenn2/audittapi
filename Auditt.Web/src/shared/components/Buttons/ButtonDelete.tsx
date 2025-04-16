import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMinus, } from '@fortawesome/free-solid-svg-icons';
export const ButtonDelete = ({ id, onDelete }: { id: number; onDelete: (e: React.MouseEvent<HTMLButtonElement>, id: number) => void }) => {
  return (
    <button className="w-10 h-10 rounded-full bg-red-300  border-red-400 flex items-center justify-center hover:border-red-500 mr-2 cursor-pointer"
      onClick={(e) => onDelete(e, id)}>
      <FontAwesomeIcon icon={faMinus} className='text-red-800 border-red-400' />
    </button>
  );
};
export default ButtonDelete;


