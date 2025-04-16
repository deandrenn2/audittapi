export const AdhesionMeasurement = () => {
    return(
        <div>
            <div className="flex space-x-4 mb-4">
                <div className="flex-1 p-8">
                    <h2 className="text-2xl font-semibold mb-4">Medición de Adhesión</h2>
                    <div className="grid grid-cols-3">
                        <div className="grid grid-cols-4 gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Nombre</div>
                        <div className="grid grid-cols-4 gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Preguntas</div>
                        <div className="grid grid-cols-4 gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 rounded mr-2 mb-2">Opciones</div>
                    </div>
                    <div className="grid grid-cols-3">
                        <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2"> Instrumento HTA</div>
                        <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">80</div>
                        <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2"></div>
                    </div>
                </div>
            </div>
        </div>
    )
}