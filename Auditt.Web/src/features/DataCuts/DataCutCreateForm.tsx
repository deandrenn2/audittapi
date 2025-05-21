import { useRef } from "react";
import { useDataCuts } from "./useDataCuts";

type formData = {
    name: string;
    maxHistory: string;
    initialDate: string;
    finalDate: string;
}
export const DataCutCreateForm = ({ idInstitution }: { idInstitution: string }) => {
    const { createDataCut } = useDataCuts();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries()) as unknown as formData;
        const newDataCut = {
            name: data.name,
            maxHistory: Number(data.maxHistory),
            initialDate: new Date(data.initialDate),
            finalDate: new Date(data.finalDate),
            institutionId: Number(idInstitution),
        };

        const response = await createDataCut.mutateAsync(newDataCut);
        if (response.isSuccess) {
            refForm.current?.reset();
        }

    }

    return (
        <div>
            <form ref={refForm} onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Nombre</label>
                    <input name="name" type="text" 
                    className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Máx Historias</label>
                    <input name="maxHistory" type="number"
                     className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Fecha Inicial</label>
                    <input name="initialDate" type="date" 
                    className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full" />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Fecha Final</label>
                    <input name="finalDate" type="date"
                     className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full" />
                </div>
                <div className="mt-4">
                    <button type="submit" className=" bg-[#392F5A] hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold">
                        {createDataCut.isPending ? "Creando..." : "Crear"}
                    </button>
                </div>
            </form>
        </div>
    );
}