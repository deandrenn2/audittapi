import { useState } from "react";
import ButtonDetail from "../../shared/components/Buttons/ButtonDetail";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { Direction } from "../../shared/components/OffCanvas/Models";
import { GuidesForm } from "./GuidesForm";

export const Guide = () => {
    const [visible, setVisible] = useState(false);
    
    const handleClick = () => {
        setVisible(true);
    };
    const handleClose = () => {
        setVisible(false);
    }

    return (
        <div className="flex p-8">
            <div>
                <h2 className="text-2xl font-semibold mb-6 mr-2">Instrumentos o GUIAS</h2>
                
                <button onClick={handleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                    Crear Instrumento
                </button>

                <div>
                    <div className="grid grid-cols-3">
                        <div className="grid grid-cols-3 gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Nombre</div>
                        <div className="grid grid-cols-3 gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Preguntas</div>
                        <div className="grid grid-cols-3 gap-3 font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">Opciones</div>
                    </div>

                    <div className="bg-white px-2 py-2 border border-gray-200">
                        <div className="grid grid-cols-3">
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2"> Instrumento HTA</div>
                            <div className="grid grid-cols-3 gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">80</div>
                            <div className="flex justify-center">
                            <ButtonDetail url={"IntrumentsForm"} />
                            </div>

                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2"> Instrumento DNT</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">43</div>
                            <div className=" flex justify-center">
                            <ButtonDetail url={""} />
                            </div>

                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2"> Instrumento DyC</div>
                            <div className=" gap-3 text-sm bg-white px-2 py-2 border border-gray-300 mr-2">34</div>
                            <div className=" flex justify-center">
                                <ButtonDetail url={""} />
                            </div>
                        </div>
                    </div>
                </div>
                <OffCanvas titlePrincipal='Crear Instrumentos o GUIAS' visible={visible} xClose={handleClose} position={Direction.Right}  >
                    <GuidesForm/>
                </OffCanvas>
            </div>
        </div>

    );
};