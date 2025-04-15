
import { LinkClients } from "../Dashboard/LinkClients";

export const Patients = () => {

    return (
        <div className="flex">
            <div className="flex space-x-4 mb-4">
                <div className="flex-1 p-8">
                    <div className="flex space-x-8 text-lg font-medium mb-4">
                     <LinkClients/> 
                    </div>

                    <h2 className="text-3xl font-semibold mb-4">Pacientes o historias </h2>

                    <div className=" grid grid-cols-4">
                        <div className=" gap-3   font-semibold bg-gray-300  text-gray-800 px-1 py-1 rounded mr-2 mb-2">IdPaciente</div>
                        <div className=" gap-3  font-semibold bg-gray-300  text-gray-800 px-1 py-1 rounded mr-2 mb-2">Fecha Nacimiento</div>
                        <div className=" gap-3   font-semibold bg-gray-300  text-gray-800 px-1 py-1 rounded mr-2 mb-2">Eps</div>
                        <div className=" gap-3  font-semibold bg-gray-300 text-sm text-gray-800 px-2 py-1 rounded mb-2">Opciones</div>
                    </div>

                    <div className="grid grid-cols-4">
                        <div className="grid grid-cols-2 gap-3  bg-white px-2 py-1 border border-gray-300 mr-2 ">1039094744</div>
                        <div className="grid grid-cols-2 gap-3  bg-white px-2 py-1 border border-gray-300 mr-2 ">02-02-1990</div>
                        <div className="grid grid-cols-2 gap-3  bg-white px-2 py-1 border border-gray-300 mr-2 ">Nueva EPS</div>
                        <div className="grid grid-cols-2 gap-3  bg-white px-2 py-1 border border-gray-300 mr-2 "></div>
                    </div>
                </div>
            </div>

        </div>

    );
}