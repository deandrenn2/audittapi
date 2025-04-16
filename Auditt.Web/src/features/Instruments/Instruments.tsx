export const Instruments = () => {
   
    const handleClick = () => {
}

    return (
    <div className="flex">
    <div className="flex-1 flex">
        <div className="flex-1 p-8">

            <h2 className="text-2xl font-semibold mb-4">Instrumentos o GUIAS</h2>
            <button onClick={handleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
             Instrumentos o GUIAS
            </button>

            <div>
                <div className="grid grid-cols-3 font-semibold bg-gray-300 text-sm text-gray-800 px-2 py-1 ">
                    <div className="">Nombre</div>
                    <div className="">Preguntas</div>
                    <div className="">Opciones</div>
                    
                </div>

                <div className="grid grid-cols-3 gap-2 text-sm bg-white px-2 py-2 border border-gray-300">
                    <div className="  bg-white px-2 py-2 border border-gray-300  "> Instrumento HTA</div>
                    <div className="  bg-white px-2 py-2 border border-gray-300 ">80</div>
                    <div className="  bg-white px-2 py-2 border border-gray-300 "></div>
                
                    <div className="  bg-white px-2 py-2 border border-gray-300 "> Instrumento DNT</div>
                    <div className="  bg-white px-2 py-2 border border-gray-300 ">43</div>
                    <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 "></div>

                    <div className="  bg-white px-2 py-2 border border-gray-300 mr-2"> Instrumento DyC</div>
                    <div className="  bg-white px-2 py-2 border border-gray-300 mr-2">34</div>
                    <div className="  bg-white px-2 py-2 border border-gray-300 mr-2"></div>
                
                </div>
            </div>
        </div>
    </div>
    </div>

   );
};