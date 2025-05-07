import { useEffect, useRef, useState } from "react";
import { useDataCuts } from "./useDataCuts";
import { DataCutModel } from "./DataCutModels";

export const DataCutUpdateForm = ({ dataCut }: { dataCut: DataCutModel }) => {
    const { updateDataCut } = useDataCuts();
    const [DataCut, setDataCut] = useState<DataCutModel>(dataCut);
    const refForm = useRef<HTMLFormElement>(null);

    useEffect(() => {
        if (dataCut?.id) {
            setDataCut(dataCut);
        }
    }, [dataCut]);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await updateDataCut.mutateAsync(DataCut);
        if (response.isSuccess) {
            refForm.current?.reset();
        }
        console.log('no hay datos para actualizar', DataCut)
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDataCut({
            ...DataCut,
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
                        value={DataCut.name}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Máx Historias</label>
                    <input
                        name="maxHistory"
                        type="number"
                        value={DataCut.maxHistory}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Fecha Inicial</label>
                    <input
                        name="initialDate"
                        type="date"
                        value={new Date(DataCut.initialDate).toISOString().substring(0, 10)}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Fecha Final</label>
                    <input
                        name="finalDate"
                        type="date"
                        value={new Date(DataCut.finalDate).toISOString().substring(0, 10)}
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
