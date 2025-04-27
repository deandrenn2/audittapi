import { useState } from "react";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { LinkSettings } from "../../Dashboard/LinkSenttings";
import { UserCreate } from "./UsersCreate";
import { Direction } from "../../../shared/components/OffCanvas/Models";

export const Users = () => {
    const [visible, setVisible] = useState(false);

    const hadbleClick = () => {
        setVisible(true);
    }

    const handleClose = () => {
        setVisible(false);
    }

    return (
        <div className="p-6">
            <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                <LinkSettings />
            </div>

            <div>
                <button onClick={hadbleClick} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-4 py-2 rounded-lg font-semibold mb-2">
                    Crear Usuario
                </button>
            </div>
            <div>
                <div className="flex items-center space-x-6 border p-4 rounded-xl shadow-md w-max">

                    <div className="w-14 h-14 rounded-full bg-red-200"></div>

                    <div className="flex flex-col">
                        <span className="font-semibold text-sm text-gray-800 uppercase">Deimer Andrés Núñez Novoa</span>
                        <span className="text-sm text-red-500">deandre.nn@gmail.com</span>
                    </div>


                    <div className="font-bold text-sm text-indigo-900 uppercase">Super Admin</div>

                    <div className="flex items-center space-x-1">
                        <div className="w-4 h-4 rounded-full bg-lime-500"></div>
                        <span className="text-sm font-semibold text-lime-600">Activo</span>
                    </div>

                    <div className="flex items-center space-x-2">
                        <div className="w-8 h-8 flex items-center justify-center rounded-full border border-blue-300 text-blue-900">

                            <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                            </svg>
                        </div>
                        <div className="w-8 h-8 flex items-center justify-center rounded-full border border-blue-300 text-blue-900">

                            <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a4 4 0 10-8 0 4 4 0 008 0zM7 10h.01M7 14h10v6H7z" />
                            </svg>
                        </div>
                        <div className="w-8 h-8 flex items-center justify-center rounded-full bg-green-200 text-green-600">

                            <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536M9 11l6.364-6.364a1.5 1.5 0 112.121 2.121L11 13l-4 1 1-4z" />
                            </svg>
                        </div>
                    </div>
                </div>
            </div>

            <OffCanvas titlePrincipal='Crear Usuario' visible={visible} xClose={handleClose} position={Direction.Right}  >
                <UserCreate />
            </OffCanvas>
        </div>
    );
}