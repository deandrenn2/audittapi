import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconDefinition } from "@fortawesome/free-solid-svg-icons";

interface MenuItemProps {
  path: string;
  icon: IconDefinition;
  text: string;
}

export const MenuItem = ({ path, icon, text, }: MenuItemProps) => {
  return (
    <li className="block pb-1 group">
      <NavLink
        to={path}
        end
        className={({ isActive }) =>
          `mt-1 font-semibold px-4 py-2 flex items-center gap-2 transition-colors duration-300 ${
            isActive ? "bg-[#FF677D] text-white" : "text-gray-300 hover:bg-gray-700"
          }`
        }
      >
        {({ isActive }) => (
          <>
            <FontAwesomeIcon
              icon={icon}
              className={`transition-colors duration-300 w-6 ${
                isActive ? "text-white" : "text-gray-300"
              }`}
            />
            <span>{text}</span>
          </>
        )}
      </NavLink>
    </li>
  );
};

