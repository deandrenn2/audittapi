import { useState } from "react";
import { ClientSelect } from "../Clients/ClientSelect";
import { Option } from "../../shared/model";
import { SingleValue } from "react-select";
import { Link } from "react-router-dom";

export const Assessments = () => {
    const [selectedClient, setSelectedClient] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un cliente",
    }));

    const handleChangeClient = (newValue: SingleValue<Option>) => {
        setSelectedClient({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    return (
        <div className="flex-1 p-8">
            <div className="">
                <div className="flex items-center space-x-4 mb-4">
                    <span className="font-medium">IPS</span>
                    <ClientSelect className="w-lg" selectedValue={selectedClient} xChange={handleChangeClient} isSearchable={true} />
                </div>

            </div>
            <h1 className="text-2xl font-semibold mb-4">Evaluaciones o auditorias</h1>
            <Link to={'/Assessments/Create'} title='Crear' className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                Realizar valoraciones
            </Link>

            <div>
                <div className="grid grid-cols-[1fr_1fr_1fr_1fr_1fr]">
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">HISTORIA</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">PROFECIONAL</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">FECHA DE ATENCION</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">FECHA DE ATENCION</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                </div>

                <div className="bg-white px-2 py-2 border border-gray-200">
                    <div className="grid grid-cols-[1fr_1fr_1fr_1fr_1fr] hover:bg-[#F4EDEE] transition-colors">
                        <div className=" gap-3 text-sm px-2 py-2 border border-gray-300">1er Trimestre 2025</div>
                        <div className=" gap-3 text-sm px-2 py-2 border border-gray-300 text-center">220</div>
                        <div className=" gap-3 text-sm px-2 py-2 border border-gray-300 text-center">01-01-2025</div>
                        <div className=" gap-3 text-sm px-2 py-2 border border-gray-300 text-center">31-03-2025</div>
                        <div className=" flex justify-center border border-gray-300">
                        </div>
                    </div>
                </div>
            </div>
        </div>

    );
}