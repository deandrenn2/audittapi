import { useState } from "react";
import ButtonDetail from "../../shared/components/Buttons/ButtonDetail";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { Direction } from "../../shared/components/OffCanvas/Models";
import { GuidesCreate } from "./GuidesCreate";
import { useGuide } from "./useGuide";
import ButtonDelete from "../../shared/components/Buttons/ButtonDelete";
import Swal from "sweetalert2";
import { Bar } from "../../shared/components/Progress/Bar";
import { GuideModel } from "./GuideModel";
import { GuideUpdate } from "./GuideUpdate";
import { ButtonPlay } from "../../shared/components/Buttons/ButtonPlay";

export const Guide = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setUpdateVisible] = useState(false);
    const { guides, queryGuide, deleteGuide } = useGuide();
    const [guide, setGuide] = useState<GuideModel>();
    const [idGuide, setIdGuide] = useState(0);


    const handleGuideDetail = (guideSelected: GuideModel) => {
        setGuide(guideSelected);
        setUpdateVisible(true);
        setIdGuide(idGuide)
    };

    function handleDelete(e: React.MouseEvent<HTMLButtonElement>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar esta Equivalencia?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteGuide.mutateAsync(id);
            }
        })
    }

    if (queryGuide.isLoading) return <Bar />;

    return (
        <div className="flex p-8">
            <div>
                <h2 className="text-2xl font-semibold mb-6 mr-2">Instrumentos o GUIAS</h2>

                <button onClick={() => setVisible(true)} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                    Crear Instrumento
                </button>

                <div>
                    <div className="grid grid-cols-4">
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Nombre</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Descripción</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 ">Preguntas</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">Opciones</div>
                    </div>

                    <div className="bg-white px-2 py-2 border border-gray-200">
                        {guides?.map((guide) => (
                            <div className="grid grid-cols-4" key={guide.id}>
                                <div className="text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">{guide.name}</div>
                                <div className="text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">{guide.description}</div>
                                <div className="text-sm bg-white px-2 py-2 border border-gray-300 mr-2 mb-2">80</div>
                                <div className="flex justify-center">
                                    <div onClick={() => handleGuideDetail(guide)}>
                                        <ButtonPlay />
                                    </div>
                                    <ButtonDetail url={"Questions"} />

                                    <ButtonDelete id={guide.id ?? 0} onDelete={handleDelete} />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
                <OffCanvas titlePrincipal='Crear Instrumentos' visible={visible} xClose={() => setVisible(false)} position={Direction.Right}>
                    <GuidesCreate />
                </OffCanvas>
                {guide && (
                    <OffCanvas titlePrincipal='Detalle Instrumentos' visible={visibleUpdate} xClose={() => setUpdateVisible(false)} position={Direction.Right}>
                        <GuideUpdate data={guide} />
                    </OffCanvas>
                )}
            </div>
        </div>
    );
};
