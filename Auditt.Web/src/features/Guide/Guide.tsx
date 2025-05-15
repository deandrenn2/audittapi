import { useState } from "react";
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
import { ButtonUpdate } from "../../shared/components/Buttons/ButtonDetail";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export const Guide = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setUpdateVisible] = useState(false);
    const { guides, queryGuide, deleteGuide } = useGuide();
    const [guide, setGuide] = useState<GuideModel>();
    const [searGuide, setSearGuide] = useState('');

    const handleGuideDetail = (guideSelected: GuideModel) => {
        setGuide(guideSelected);
        setUpdateVisible(true);

    };

    function handleDelete(e: React.MouseEvent<HTMLButtonElement>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar esta Guide?',
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

    if (queryGuide.isLoading) return <Bar />

    const normalizeText = (text: string) =>
        text.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();

    const search = normalizeText(searGuide.trim());
    const filterdGuide = guides?.filter(guide => {
        const fields = `${guide?.name}`;
        const words = normalizeText(fields).split(/\s+/);
        return words.some(word => word.startsWith(search));
    })

    return (
        <div className="p-6 w-full">
            <div>
                <div className="flex">
                     <h2 className="text-2xl font-semibold mr-2 mb-5 ">Instrumentos o Guias</h2>
                    <div className="relative">
                        <div className=" inline-flex  mr-2">
                            <input type="text"
                                value={searGuide}
                                onChange={(e) => setSearGuide(e.target.value)}
                                placeholder="Buscar Cliente"
                                className="border rounded px-3 py-1 transition duration-200 border-gray-300 hover:border-indigo-500 
                                 hover:bg-gray-50 focus:outline-none focus:ring-2 text-center focus:ring-indigo-400"/>
                            <FontAwesomeIcon icon={faMagnifyingGlass} className="fas fa-search absolute left-3 top-3 text-gray-400" />
                        </div>
                    </div>
                    <button onClick={() => setVisible(true)} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 rounded-lg font-semibold mb-5 mr-2">
                        Crear Instrumento
                    </button>
                </div>

                <div>
                    <div className="grid grid-cols-4">
                        <div className=" font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">Nombre</div>
                        <div className=" font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">Descripcion</div>
                        <div className=" font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">Preguntas</div>
                        <div className=" font-semibold bg-gray-300 text-gray-800 px-2 py-1 text-center">Opciones</div>
                    </div>

                    <div className="bg-white px-2 py-2 border border-gray-200">
                        {filterdGuide?.map((guide) => (
                            <div className="grid grid-cols-4 hover:bg-[#F4EDEE] transition-colorsl"
                                key={guide.id}>
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">{guide.name}</div>
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">{guide.description}</div>
                                <div className="text-sm px-2 py-2 border border-gray-300 text-center">80</div>
                                <div className="flex justify-center text-sm px-2 border border-gray-300 py-1">
                                    <div onClick={() => handleGuideDetail(guide)}>
                                        <ButtonUpdate />
                                    </div>
                                    <div>
                                        <ButtonPlay url={"Questions"}/>
                                    </div>
                                    <ButtonDelete id={guide.id ?? 0} onDelete={handleDelete} />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
                
                <OffCanvas titlePrincipal="Crear Instrumentos" visible={visible} xClose={() => setVisible(false)} position={Direction.Right}>
                    <GuidesCreate />
                </OffCanvas>
                {guide && (
                    <OffCanvas titlePrincipal="Detalle Instrumentos" visible={visibleUpdate} xClose={() => setUpdateVisible(false)} position={Direction.Right}>
                        <GuideUpdate data={guide} />
                    </OffCanvas>
                )}
            </div>
        </div>
    );
};
