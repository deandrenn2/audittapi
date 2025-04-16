import ButtonDetail from "../../shared/components/Buttons/ButtonDetail";
export const QuarterlyCuts = () => {

    return (
        <div className="flex-1 p-8">

            <div className="flex justify-between items-center mb-6">
                <div className="flex items-center space-x-4">
                    <span className="font-medium">IPS</span>
                    <select className="border rounded px-3 py-1">
                        <option>HOSPITAL SAN SEBASTIAN</option>
                    </select>
                </div>

            </div>

            <h1 className="text-2xl font-semibold mb-4">Cortes trimestrales de auditoria</h1>
            
            <div>
                <div className="grid grid-cols-6">
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Nombre</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Máx Historias</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Fecha Inicial</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Fecha Final</div>
                    <div className="gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Opciones</div>
                </div>

                <div className="grid grid-cols-6">
                    <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">1er Trimestre 2025</div>
                    <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">220</div>
                    <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">01-01-2025</div>
                    <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">31-03-2025</div>
                    <div> <ButtonDetail url={"/InstrumentsDetail"} /></div>
                </div>
            </div>
        </div>

    );
}