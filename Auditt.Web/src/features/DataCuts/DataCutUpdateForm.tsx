import { useEffect, useRef, useState } from "react";
import { useDataCuts } from "./useDataCuts";
import { DataCutModel } from "./DataCutModels";

export const DataCutUpdateForm = ({ dataCut }: { dataCut: DataCutModel }) => {
    const { updateDataCut } = useDataCuts();
    const [currentDataCut, setDataCut] = useState<DataCutModel>(dataCut);
    const refForm = useRef<HTMLFormElement>(null);

    useEffect(() => {
        if (dataCut?.id) {
            setDataCut(dataCut); // Asegúrate de que `dataCut` tenga un `id` válido
        }
    }, [dataCut]);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updateDataCut.mutateAsync(currentDataCut);
        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDataCut({
            ...currentDataCut,
            [e.target.name]: e.target.value,
        });
    };

    return (
        <div>
            <form ref={refForm} onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Nombre</label>
                    <input
                        name="name"
                        type="text"
                        value={currentDataCut.name}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Máx Historias</label>
                    <input
                        name="maxHistory"
                        type="number"
                        value={currentDataCut.maxHistory}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Fecha Inicial</label>
                    <input
                        name="initialDate"
                        type="date"
                        value={new Date(currentDataCut.initialDate).toISOString().substring(0, 10)}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Fecha Final</label>
                    <input
                        name="finalDate"
                        type="date"
                        value={new Date(currentDataCut.finalDate).toISOString().substring(0, 10)}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div className="mt-4">
                    <button
                        type="submit"
                        className="bg-[#392F5A] hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold"
                    >
                        {updateDataCut.isPending ? "Actualizando..." : "Actualizar"}
                    </button>
                </div>
            </form>
        </div>
    );
};
