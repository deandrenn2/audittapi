import { useState, useEffect } from "react";
import { EquivalenceModel } from "./EquivalenceModel";
import { useEquivalence } from "./useEquivalence";

export const EquivalenceUpdate = ({ data }: { data: EquivalenceModel }) => {
    const { updateEquivalence, } = useEquivalence();
    const [equivalence, setEquivalence] = useState<EquivalenceModel>(data);

    useEffect(() => {
        setEquivalence(data);
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
            alert("Por favor complete todos los campos");
            return;
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
                    className="border px-3 py-2 rounded w-full"
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
                    className="border px-3 py-2 rounded w-full"
                />
            </div>
            <button
                onClick={handleSubmit}
                className="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-800"
            >
                Actualizar Equivalencia
            </button>
        </div>
    );
};
