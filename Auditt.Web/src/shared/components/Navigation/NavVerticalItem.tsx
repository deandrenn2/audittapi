import { NavLink } from "react-router-dom";

 export const SimpleNavItem = ({
  to,
  title,
}: {
  to: string;
  title: string;
}) => {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        `border-b-2 pb-1 px-2 transition-colors duration-300 text-2xl cursor-pointer mr-2 ${
          isActive ? "text-[#FF677D] border-[#FF677D]" : "text-gray-800 border-transparent"
        }`
      }
    >
      {title}
    </NavLink>
  );
};

 