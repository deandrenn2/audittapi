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
            <h1 className="text-6xl font-bold text-gray-800">
                Bienvenido a <span className="text-pink-500 font-bold">Auditt</span><span className="text-gray-800">Api</span>
            </h1>

            <div className="mt-10 flex space-x-3 gap-3 ">
                <button className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">
                    Crear Clientes
                </button>

                <button className=" bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">Crear
                    Pacientes</button>
                
                <button className=" bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold">Crear
                    Instrumentos
                </button>
            </div>

            <div className="mt-4 grid grid-cols-3 scale-90">
                <div className=" bg-pink-400 text-white rounded-2xl p-2 text-center mr-2">
                    <p className=" font-bold">Total Valoraciones</p>
                    <p className="text-3xl font-bold mt-2">250</p>
                </div>
                <div className="bg-pink-300 text-white rounded-2xl p-2 text-center mr-2">
                    <p className="font-bold">Total Pacientes</p>
                    <p className="text-5xl font-bold mt-2">157</p>
                </div>
                <div className="bg-pink-300 text-indigo-900 rounded-2xl p-2 text-center">
                    <p className="font-bold">Total Profesionales</p>
                    <p className="text-5xl font-bold mt-2">234</p>
                </div>
            </div>

            <div className="mt-10">
                <p className="font-bold">Evaluaciones</p>
                <p className="text-sm mt-1 mb-2">Filtrar por ID Paciente</p>
                <input type="text" value="1039094780" className="border border-gray-400 rounded-lg px-4 py-2" />
            </div>

            <div className="mt-6 space-y-3">
                <div className="h-6 bg-gray-200 rounded"></div>
                <div className="h-6 bg-gray-200 rounded"></div>
            </div>
        </div>


    )
};