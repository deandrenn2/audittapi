import ButtonDelete from "../../shared/components/Buttons/ButtonDelete";
import ButtonDetail from "../../shared/components/Buttons/ButtonDetail";
import { LinkClients } from "../Dashboard/LinkClients";
export const Patients = () => {

    return (
        <div className="flex">
            <div className="">
                <div className="flex-1 p-8">
                    <div className="flex space-x-8 text-lg font-medium mb-4">
                        <LinkClients />
                    </div>

                    <h2 className="text-2xl font-semibold mb-4">Pacientes o historias </h2>
                    <div>
                        <div className="grid grid-cols-4">
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1  mb-2">IdPaciente</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1  mb-2 ">Fecha Nacimiento</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1  mb-2 ">Eps</div>
                            <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1  mb-2 text-center">Opciones</div>
                        </div>

                        <div className="bg-white px-2 py-2 border border-gray-200">
                            <div className="grid grid-cols-4">
                                <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">1039094744</div>
                                <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">02-02-1990</div>
                                <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">Nueva EPS</div>
                                <div className="flex justify-center">
                                    <ButtonDelete id={0} onDelete={undefined}/>
                                    <ButtonDetail url={""} /></div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>

    );
}