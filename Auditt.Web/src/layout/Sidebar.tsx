import { faHouse, faUser, faUsers, faGear, faClipboardCheck, faBoxesStacked } from '@fortawesome/free-solid-svg-icons';
import { MenuItem } from './MenuItem';
export const Sidebar = () => {
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
                  <MenuItem icon={faBoxesStacked} path='/Quarterly' text='Cortes Trimestrales' />
                  <MenuItem icon={faClipboardCheck} path='/Measurement' text='Medición de Adherencia' />
                  <MenuItem icon={faGear} path='/Settings' text='Configuraciones' />
               </ul>
            </nav>
         
         </div>
      </div>
   );
};
