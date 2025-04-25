export const QuarterlyForm = () => {
    return (
        <div>
        <div className="flex">
            <main className="">
                <div className="space-y-2">
                <label className="block font-medium mb-1">IPS</label>
                    <div className="flex justify-between">
                        
                        <select className="border border-black px- py-2 rounded w-1/2">
                            <option>HOSPITAL SAN SEBASTIAN</option>
                        </select>
                        <div className="flex justify-end mt-2">
                            <button className="bg-indigo-500 hover:bg-indigo-900 text-white px-4 py-2 rounded">
                                Ir a Indicadores e Informes
                            </button>
                        </div>
                    </div>

                    <div>
                        <label className="block font-medium mb-1">Corte de Auditoria</label>
                        <select className="border border-black px-3 py-2 rounded w-80">
                            <option>1er trimestre 2025</option>
                        </select>
                    </div>

                    <div>
                        <label className="block font-medium mb-1">Profesional Evaluado</label>
                        <select className="border border-black px-3 py-2 rounded">
                            <option>JUAN PEREZ</option>
                        </select>
                    </div>
                    <div className="grid-cols-3 space-y-4">
                        <div className="space-y-2">
                        
                            <div className="flex items-center justify-end ">
                            <label className="font-medium">ID Paciente</label>
                                <div className="flex items-center gap-2">
                                    <div className="border border-black px-3 py-1 rounded">1039094780</div>
                                    <button className="bg-indigo-500 hover:bg-indigo-900 text-white px-3 py-1 rounded">Diligenciar</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="flex justify-between items-start ">
                        <div className="flex items-center mb-10">
                            <select className="border border-black px-3 py-2 rounded mr-2">
                                <option>Instrumento HTA</option>
                            </select>
                            <button className="bg-green-500 hover:bg-green-700 text-white w-8 h-8 text-xl rounded-full flex items-center justify-center">
                                +
                            </button>
                        </div>

                        
                        <div className="flex flex-col items-end gap-1 mr-2">
                            <div className="flex gap-6 font-semibold">
                                <span>Edad</span>
                                <span>Fecha</span>
                                <span>EPS</span>
                            </div>
                            <div className="flex gap-2 font-semibold  ">
                                <span className="bg-zinc-300 mr-1">25</span>
                                <span className="bg-zinc-300 mr-1">12/01/2025</span>
                                <span className="bg-zinc-300 mr-1">Eps</span>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="mt-10 flex gap-2">
                    <div className="bg-green-100 border border-green-300 px-4 py-3 rounded flex-1 flex flex-col justify-between">
                        <p className="text-sm text-gray-800 mb-2">
                            En cuidado primario y rutinario de pacientes con HTA estadio I/no complicada se recomienda no hacer
                            fundoscopia para valoración de daño micro vascular. (FUERTE EN CONTRA= efectos indeseables)
                        </p>
                        <select className="border border-black px-2 py-1 rounded ">
                            <option>Cumple</option>
                        </select>
                    </div>

                    <div className="bg-green-100 border border-green-300 px-4 py-3 rounded flex-1 flex flex-col justify-between">
                        <p className="text-sm text-gray-800 mb-2">
                            En los primeros tres meses después del diagnóstico de HTA, debe descartarse lesión glomerular en muestra
                            de orina casual, evaluando la relación proteinuria/creatinuria positiva, o mediante proteinuria en tiras reactivas.
                        </p>
                        <select className="border border-black px-2 py-1 rounded w-full">
                            <option>No Cumple</option>
                        </select>
                    </div>
                </div>

                <div className="mt-6 flex gap-4">
                    <button className="bg-rose-400 hover:bg-rose-600 text-white px-6 py-2 rounded">Guardar</button>
                    <button className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded">Terminar y Guardar</button>
                </div>
            </main>
        </div>
    </div>
    )
}
