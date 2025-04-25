import { useState } from "react";
import { Direction } from "../../shared/components/OffCanvas/Models";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { DataCutCreateForm } from "./DataCutCreateForm";
import { ButtonDetail } from "../../shared/components/Buttons/ButtonDetail";
import { useDataCuts } from "./useDataCuts";
import { Bar } from "../../shared/components/Progress/Bar";
import { ClientSelect } from "../Clients/ClientSelect";
import { Option } from "../../shared/model";
import { SingleValue } from "react-select";
export const DataCuts = () => {
    const [visible, setVisible] = useState(false);
    const { queryDataCuts, dataCuts } = useDataCuts();
    const [selectedClient, setSelectedClient] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un cliente",
    }));

    const hadbleClick = () => {
        setVisible(true);
    }

    const handleChangeClient = (newValue: SingleValue<Option>) => {
        setSelectedClient({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    const handleClose = () => {
        setVisible(false);
    }

    if (queryDataCuts.isLoading)
        return <Bar />;

    return (
        <div className="flex-1 p-8 ">
            <div className="">
                <div className="flex items-center space-x-4 mb-4">
                    <span className="font-medium">IPS</span>
                    <ClientSelect className="w-lg" selectedValue={selectedClient} xChange={handleChangeClient} isSearchable={true} />
                </div>

            </div>
            <h1 className="text-2xl font-semibold mb-4">Cortes trimestrales de auditoria</h1>
            <button onClick={hadbleClick} className="bg-indigo-500 hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                Crear
            </button>

            <div>
                <div className="grid grid-cols-[2fr_1fr_1fr_1fr_2fr] w-full min-w-[700px]">
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">NOMBRE</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">MAX HISTORIAS</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">FECHA INICIAL</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1">FECHA FINAL</div>
                    <div className="font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                </div>
                <div className="bg-white px-2 py-2 border border-gray-200">
                    {dataCuts?.map((item) => (
                        <div className="grid grid-cols-[2fr_1fr_1fr_1fr_2fr] w-full min-w-[700px]">
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{item.name}</div>
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{item.maxHistory}</div>
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{item.initialDate.toString()}</div>
                            <div className="text-sm bg-white px-2 py-2 border border-gray-300">{item.finalDate.toString()}</div>
                            <div className=" flex justify-center">
                                <div>
                                    <ButtonDetail />
                                </div>
                            </div>
                        </div>
                    ))
                    }
                </div>
                <OffCanvas titlePrincipal='Crear Cortes Trimestrales' visible={visible} xClose={handleClose} position={Direction.Right}  >
                    <DataCutCreateForm idInstitution={selectedClient?.value ?? "0"} />
                </OffCanvas>
            </div>
        </div>

    );
}