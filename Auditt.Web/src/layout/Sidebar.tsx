import { faHouse, faUser, faUsers, faGear, faClipboardCheck, faBoxesStacked, faLockOpen } from '@fortawesome/free-solid-svg-icons';
import { MenuItem } from './MenuItem';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
export const Sidebar = () => {
   const urlApi = import.meta.env.VITE_API_URL;
   return (
      <div
         id="sidebar"
         className=" w-64 bg-gray-800 text-white flex flex-col">
         <div>
            <div className="flex flex-col items-center p-6 justify-center">
               <div className="">
                  <img src="" alt="Logo" className="w-16 h-16 bg-pink-300 rounded-full mb-2" />
               </div>
               <p className="text-center text-sm mb-3">Deimer Andrés...</p>
               <p className='text-center text-sm text-gray-600 '>NAVEGACIÓN</p>
            </div>

            <nav>
               <ul className="space-y-1">
                  <MenuItem icon={faHouse} path='/' text='Inicio' />
                  <MenuItem icon={faUser} path='/Clients' text='Clientes' />
                  <MenuItem icon={faUsers} path='/Guide' text='Instrumentos' />
                  <MenuItem icon={faBoxesStacked} path='/DataCuts' text='Cortes Trimestrales' />
                  <MenuItem icon={faClipboardCheck} path='/Assessments' text='Medición de Adherencia' />
                  <MenuItem icon={faGear} path='/Settings' text='Configuraciones' />
                  <a
                     onClick={() => {
                        window.location.href = `${urlApi}api/auth/google-logout`;
                     }}
                     className={`mt-1 cursor-pointer font-semibold text-gray-300 hover:bg-gray-700 rounded px-4 py-2 flex items-center gap-1`}>
                     <FontAwesomeIcon
                        icon={faLockOpen}
                        className={`'text-gray-300'} transition-colors duration-300`}
                     />
                     <span>Salir</span>
                  </a>
               </ul>
            </nav>

         </div>
      </div>
   );
};
