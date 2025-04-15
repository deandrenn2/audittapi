import { IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';

interface MenuItemProps {
  path: string; icon: IconDefinition; text: string;
}

export const MenuItem = ({ path, icon, text }: MenuItemProps) => {
  return (
    <li className="block pb-2 group">
      <Link
        to={path}
        className="mt-1 font-semibold text-gray-300 hover:bg-gray-700 rounded px-4 py-2 flex items-center gap-1">
        <FontAwesomeIcon icon={icon} />
        <span>{text}</span>
      </Link>
    </li>
  );
};
