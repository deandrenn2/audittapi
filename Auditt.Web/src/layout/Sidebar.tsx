import { faHouse, faUser, faUsers, faGear, faClipboardCheck, faBoxesStacked, faLockOpen } from '@fortawesome/free-solid-svg-icons';
import { MenuItem } from './MenuItem';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import useUserContext from '../shared/context/useUserContext';
export const Sidebar = () => {
   const urlApi = import.meta.env.VITE_API_URL;
   const { user } = useUserContext();
   return (
      <div
         id="sidebar"
         className=" w-68 bg-gray-800 text-white flex flex-col">
         <div>
            <div className="flex flex-col items-center p-4 justify-center">
               <div>
                  <img
                     src={`${user?.urlProfile}`}
                     alt="avatar"
                     className="min-w-16 rounded-full w-20 h-20" />
               </div>

               <div>
                  <h4 className="text-white font-semibold w-64 truncate overflow-hidden whitespace-nowra p-2 text-center">
                     {user?.firstName} {user?.lastName}
                  </h4>
               </div>
            </div>
            <div className='text-center text-sm  text-gray-100 bg-gray-600 py-1'></div>
            <nav>
               <ul className="space-y-1">
                  <MenuItem icon={faHouse} path='/' text='Inicio' />
                  <MenuItem icon={faUser} path='/Clients' text='Clientes' />
                  <MenuItem icon={faUsers} path='/Guide' text='Instrumentos' />
                  <MenuItem icon={faBoxesStacked} path='/DataCuts' text='Cortes Trimestrales' />
                  <MenuItem icon={faClipboardCheck} path='/Assessments' text='Medición de Adherencia' />
                  <MenuItem icon={faGear} path='/Settings' text='Configuraciones' />
                  <li
                     onClick={() => {
                        window.location.href = `${urlApi}api/auth/google-logout`;
                     }}
                     className={`mt-1 cursor-pointer font-semibold text-gray-300 hover:bg-gray-700 px-4 py-2 flex items-center gap-2`}>
                     <FontAwesomeIcon
                        icon={faLockOpen}
                        className={`'text-gray-300'} w-6  transition-colors duration-300`}
                     />
                     <span>Salir</span>
                  </li>
               </ul>
            </nav>
         </div>
      </div>
   );
};
