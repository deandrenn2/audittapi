import { useState } from "react";
import { Direction } from "../../shared/components/OffCanvas/Models";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { DataCutCreateForm } from "./DataCutCreateForm";
import { ButtonUpdate } from "../../shared/components/Buttons/ButtonDetail";
import { useDataCuts } from "./useDataCuts";
import { Bar } from "../../shared/components/Progress/Bar";
import { ClientSelect } from "../Clients/ClientSelect";
import { Option } from "../../shared/model";
import { SingleValue } from "react-select";
import ButtonDelete from "../../shared/components/Buttons/ButtonDelete";
import Swal from "sweetalert2";
import { DataCutUpdateForm } from "./DataCutUpdateForm";

export const DataCuts = () => {
    const [visibleCreate, setVisibleCreate] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const [selectedDataCut, setSelectedDataCut] = useState<any | null>(null);
    const { queryDataCuts, dataCuts, deleteDataCut } = useDataCuts();
    const [selectedClient, setSelectedClient] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un cliente",
    }));

    const handleCreateClick = () => {
        setVisibleCreate(true);
    };

    const handleChangeClient = (newValue: SingleValue<Option>) => {
        setSelectedClient({
            value: newValue?.value,
            label: newValue?.label,
        });
    };

    const handleCloseCreate = () => {
        setVisibleCreate(false);
    };

    const handleCloseUpdate = () => {
        setVisibleUpdate(false);
        setSelectedDataCut(null);
    };

    const handleUpdateClick = (item: any) => {
        setSelectedDataCut(item);
        setVisibleUpdate(true);
    };

    function handleDelete(e: React.MouseEvent<HTMLButtonElement>, id: number): void {
        e.preventDefault();
        console.log("ID del DataCut:", id);
        Swal.fire({
            title: '¿Estás seguro de eliminar este corte?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                try {
                    await deleteDataCut.mutateAsync(id);
                    await queryDataCuts.refetch();
                    Swal.fire('¡Eliminado!', 'El corte ha sido eliminado correctamente.', 'success');
                } catch (error) {
                    Swal.fire('Error', 'Hubo un problema al eliminar el corte.', 'error');
                }
            }
        });
    }

    if (queryDataCuts.isLoading) return <Bar />;

    return (
        <div className="flex-1 p-8">
            <div>
                <div className="flex items-center space-x-4 mb-4">
                    <span className="font-medium">IPS</span>
                    <ClientSelect
                        className="w-lg"
                        selectedValue={selectedClient}
                        xChange={handleChangeClient}
                        isSearchable={true}
                    />
                </div>
            </div>

            <h1 className="text-2xl font-semibold mb-4">Cortes trimestrales de auditoría</h1>

            <button
                onClick={handleCreateClick}
                className="bg-[#392F5A] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2"
            >
                Crear
            </button>

            <div>
                <div className="grid grid-cols-[1fr_1fr_1fr_1fr_1fr]">
                    <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1">NOMBRE</div>
                    <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1">MAX HISTORIAS</div>
                    <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1">FECHA INICIAL</div>
                    <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1">FECHA FINAL</div>
                    <div className="font-semibold bg-gray-300 text-gray-800 px-2 py-1">OPCIONES</div>
                </div>

                <div className="bg-white px-2 py-2 border border-gray-200">

                    {dataCuts?.map((item) => (
                        <div key={item.id} className="grid grid-cols-[1fr_1fr_1fr_1fr_1fr] hover:bg-[#F4EDEE] transition-colors">
                            <div className="text-sm px-2 py-2 border border-gray-300">{item.name}</div>
                            <div className="text-sm px-2 py-2 border border-gray-300">{item.maxHistory}</div>
                            <div className="text-sm px-2 py-2 border border-gray-300">{item.initialDate.toString()}</div>
                            <div className="text-sm px-2 py-2 border border-gray-300">{item.finalDate.toString()}</div>
                            <div className="flex justify-center text-sm px-2  border border-gray-300">
                                <ButtonDelete id={item.id ?? 0} onDelete={handleDelete} />
                                <button
                                    onClick={() => handleUpdateClick(item)}
                                    className="text-blue-600 hover:underline text-sm"
                                >
                                    <ButtonUpdate />
                                </button>
                            </div>
                        </div>
                    ))}
                </div>

                <OffCanvas
                    titlePrincipal="Crear Cortes Trimestrales" visible={visibleCreate} xClose={handleCloseCreate} position={Direction.Right}>
                    <DataCutCreateForm idInstitution={selectedClient?.value ?? "0"} />
                </OffCanvas>

                {selectedDataCut && (
                    <OffCanvas
                        titlePrincipal="Actualizar Cortes" visible={visibleUpdate} xClose={handleCloseUpdate} position={Direction.Right}>
                        <DataCutUpdateForm  dataCut={selectedDataCut} />
                    </OffCanvas>
                )}
            </div>
        </div>
    );
};
