import { useEffect, useState } from "react";
import { DashboardResponseModel } from "./DashboardModel";
import { getDashboard } from "./DashboardServices";
import { PatientsCreate } from "../Clients/Patients/PatientsCreate";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { Direction } from "../../shared/components/OffCanvas/Models";
import { ClientCreate } from "../Clients/ClientCreate";
import { GuidesCreate } from "../Guide/GuidesCreate";

export const DashboradStatistcs = () => {
    const [dashboarData, setDashboardData] = useState<DashboardResponseModel | undefined>(undefined);
    const [visible, setVisible] = useState(false);
    const [visibleClient, setVisibleClient] = useState(false);
    const [visibleInstrument, setVisibleInstrument] = useState(false);
    
    
    useEffect(() => {
        const fetchDashaboard = async () => {
            const response = await getDashboard();
            if (response.isSuccess) {
                setDashboardData(response.data);
            }
        };
        fetchDashaboard();
    }, []);


    const handleClose = () => {
        setVisible(false);
        setVisibleClient(false);
        setVisibleInstrument(false);
    };

    return (
        <div className="flex-1 p-10 ">
            <h1 className="text-8xl ">
                <span className="text-[#392F5A] font-bold">Bienvenido a 
                </span> <span className="text-[#FFB3BA] font-bold">Auditt</span><span className="text-[#392F5A]">Api</span>
            </h1>

            <div className="mt-10 flex space-x-3 gap-3">
                <button onClick={() => setVisibleClient(true)} className="cursor-pointer hover:bg-indigo-900 bg-[#392F5A] text-white px-6 py-2 rounded-lg font-semibold">
                    Crear Clientes
                </button>

                <button onClick={() => setVisible(true)} className="cursor-pointer bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">
                    Crear Pacientes
                </button>

                <button onClick={() => setVisibleInstrument(true)} className="cursor-pointer bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">
                    Crear Instrumentos
                </button>
            </div>

            <div className="mt-4 flex justify-center  ">
                <div className=" bg-[#FF677D] text-white rounded-2xl p-2 text-center mr-2">
                    <p className=" font-bold text-4xl">Total Valoraciones</p>
                    <p className="text-6xl font-bold mt-2">250</p>
                </div>
                <div className="bg-[#FFB3BA] text-white rounded-2xl p-2 text-center mr-2">
                    <p className="font-bold text-5xl">Total Pacientes</p>
                    <p className="text-6xl font-bold mt-2">157</p>
                </div>
                <div className="bg-[#FFB3BA] text-indigo-900 rounded-2xl p-2 text-center text-2xl">
                    <p className="font-bold text-4xl">Total Profesionales</p>
                    <p className="text-6xl font-bold mt-2">234</p>
                </div>
            </div>

            <div className="mt-10">
                <p className="font-bold text-2xl">Evaluaciones</p>
                <p className="text-sm mt-1 mb-2">Filtrar por ID Paciente</p>
                <input type="text" value="1039094780" className="text-center w-58 border border-gray-300 rounded px-3 py-2 transition duration-200 hover:border-indigo-500
                         hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400"/>
            </div>

            <div className="mt-6 space-y-3">
                <div className="h-7 bg-gray-200 rounded"></div>
                <div className="h-7 bg-gray-200 rounded"></div>
            </div>
            
            <OffCanvas titlePrincipal="Crear Cliente" visible={visibleClient} xClose={handleClose} position={Direction.Right}>
                <ClientCreate />
            </OffCanvas>

            <OffCanvas titlePrincipal="Crear Paciente" visible={visible} xClose={handleClose} position={Direction.Right}>
                <PatientsCreate />
            </OffCanvas>

            <OffCanvas titlePrincipal="Crear Instrumento" visible={visibleInstrument} xClose={handleClose} position={Direction.Right}>
                <GuidesCreate />
            </OffCanvas>

        </div>
    )
};