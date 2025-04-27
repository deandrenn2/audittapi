import { useEffect, useState } from "react";
import { DashboardResponseModel } from "./DashboardModel";
import { getDashboard } from "./DashboardServices";

export const DashboradStatistcs = () => {
    const [dashboarData, setDashboardData] = useState<DashboardResponseModel | undefined>(undefined);
    
    
    useEffect(() => {
        const fetchDashaboard = async () => {
            const response = await getDashboard();
            if (response.isSuccess) {
                setDashboardData(response.data);
            }
        };
        fetchDashaboard();
    }, []);

    return (
        <div className="flex-1 p-10 ">
            <h1 className="text-8xl ">
                <span className="text-[#392F5A] font-bold">Bienvenido a 
                </span> <span className="text-[#FFB3BA] font-bold">Auditt</span><span className="text-[#392F5A]">Api</span>
            </h1>

            <div className="mt-10 flex space-x-3 gap-3 ">
                <button className="hover:bg-indigo-900 bg-[#392F5A]  text-white px-6 py-2 rounded-lg font-semibold">
                    Crear Clientes
                </button>

                <button className=" bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">Crear
                    Pacientes</button>
                
                <button className=" bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">Crear
                    Instrumentos
                </button>
            </div>

            <div className="mt-4 flex justify-center  ">
                <div className=" bg-[#FF677D] text-white rounded-2xl p-2 text-center mr-2">
                    <p className=" font-bold text-3xl">Total Valoraciones</p>
                    <p className="text-6xl font-bold mt-2">250</p>
                </div>
                <div className="bg-[#FFB3BA] text-white rounded-2xl p-2 text-center mr-2">
                    <p className="font-bold text-3xl">Total Pacientes</p>
                    <p className="text-6xl font-bold mt-2">157</p>
                </div>
                <div className="bg-[#FFB3BA] text-indigo-900 rounded-2xl p-2 text-center text-2xl">
                    <p className="font-bold text-3xl">Total Profesionales</p>
                    <p className="text-6xl font-bold mt-2">234</p>
                </div>
            </div>

            <div className="mt-10">
                <p className="font-bold text-2xl">Evaluaciones</p>
                <p className="text-sm mt-1 mb-2">Filtrar por ID Paciente</p>
                <input type="text" value="1039094780" className="border border-gray-400 rounded-lg px-8 py-2"/>
            </div>

            <div className="mt-6 space-y-3">
                <div className="h-7 bg-gray-200 rounded"></div>
                <div className="h-7 bg-gray-200 rounded"></div>
            </div>

        </div>
    )
};