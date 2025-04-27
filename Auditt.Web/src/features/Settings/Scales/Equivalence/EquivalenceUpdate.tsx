import { useState, useEffect } from "react";
import { EquivalenceModel } from "./EquivalenceModel";
import { useEquivalence } from "./useEquivalence";

export const EquivalenceUpdate = ({ data }: { data: EquivalenceModel }) => {
    const { updateEquivalence, } = useEquivalence();
    const [equivalence, setEquivalence] = useState<EquivalenceModel>(data);

    useEffect(() => {
       
        const cleanData = {
            ...data,
            scale: undefined,
        };
        setEquivalence(cleanData);
    }, [data]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setEquivalence(prev => ({
            ...prev,
            [name]: name === "value" ? Number(value) : value
        }));
    };

    const handleSubmit = async () => {
        if (!equivalence.name || equivalence.value === undefined) {

           
        }
        await updateEquivalence.mutateAsync(equivalence);
    };

    return (
        <div>
            <div className="mb-2">
                <label>Nombre</label>
                <input
                    type="text"
                    name="name"
                    value={equivalence.name}
                    onChange={handleChange}
                    placeholder="Nombre"
                    className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full"
                />
            </div>
            <div className="mb-2">
                <label>Valor</label>
                <input
                    type="number"
                    name="value"
                    value={equivalence.value}
                    onChange={handleChange}
                    placeholder="Valor"
                    className="shadow appearance-none border border-gray-300 rounded px-2 py-2 transition duration-200 hover:border-indigo-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-400 mr-2 w-full"
                />
            </div>
            <button
                onClick={handleSubmit}
                className="bg-[#392F5A] text-white px-4 py-2 rounded hover:bg-indigo-800"
            >
                Actualizar Equivalencia
            </button>
        </div>
    );
};
