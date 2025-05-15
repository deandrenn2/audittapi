import { IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link, useLocation } from 'react-router-dom';

interface MenuItemProps {
  path: string;
  icon: IconDefinition;
  text: string;
}

export const MenuItem = ({ path, icon, text }: MenuItemProps) => {
  const location = useLocation();
  const isActive = location.pathname === path;

  return (
    <li className="block pb-1 group">
      <Link
        to={path}
        className={`mt-1 font-semibold px-4 py-2 flex items-center gap-2 transition-colors duration-300 
                    ${isActive ? 'bg-[#FF677D] text-white' : 'text-gray-300 hover:bg-gray-700'}`}>
        <FontAwesomeIcon
          icon={icon}
          className={`transition-colors duration-300 w-6 ${isActive ? 'text-white' : 'text-gray-300'}`}
        />
        <span>{text}</span>
      </Link>
    </li>
  );
};
